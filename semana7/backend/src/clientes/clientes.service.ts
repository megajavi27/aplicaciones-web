import { ConflictException, Inject, Injectable, NotFoundException } from '@nestjs/common';
import { CreateClienteDto } from './dto/create-cliente.dto';
import { UpdateClienteDto } from './dto/update-cliente.dto';
import { InjectRepository } from '@nestjs/typeorm';
import { Cliente } from './entities/cliente.entity';
import { Repository } from 'typeorm';


@Injectable()
export class ClientesService {
  constructor(
    @InjectRepository(Cliente) 
    private readonly clienteRepository:Repository<Cliente>
  ) {}
  async create(createClienteDto: CreateClienteDto):Promise<Cliente> {
    const existe = await this.clienteRepository.findOne({
      where:{cedula: createClienteDto.cedula}
    })
    if(existe) {
      throw new ConflictException("Ya existe un cliente registrado con ese numero de cedula")
    }
    const cliente = this.clienteRepository.create(createClienteDto)
    return this.clienteRepository.save(cliente)
  }

  async findAll():Promise<Cliente[]> {
    return this.clienteRepository.find({
      order:{ id: 'DESC'}
    })
  }

  async findOne(id: number):Promise<Cliente> {
    const cliente = await this.clienteRepository.findOne({where: {id}})
    if(!cliente){
      throw new NotFoundException("Cliente no encontrado")
    }
    return cliente
  }

  async update(id: number, updateClienteDto: UpdateClienteDto):Promise<Cliente> {
    const cliente = await this.findOne(id)
    if(updateClienteDto.cedula && updateClienteDto.cedula !== cliente.cedula)
    {
      const existe = await this.clienteRepository.findOne({
        where: {cedula: updateClienteDto.cedula}
      })
      if (existe){
        throw new ConflictException("Ya existe un cliente con esa cedula")
      }
    }
    Object.assign(cliente, updateClienteDto)
    return this.clienteRepository.save(cliente)

  }

  async remove(id: number):Promise<{message:string}> {
    const cliete = await this.findOne(id)
    await this.clienteRepository.remove(cliete)
    return{
      message:"Cliente eliminado con exito"
    }
  }

  async getFactura(id: number) {
    const cliente = await this.findOne(id)

    const items = [
      { descripcion: 'Servicio de consultoría',  cantidad: 2, precio: 120.00 },
      { descripcion: 'Licencia de software',     cantidad: 1, precio: 250.00 },
      { descripcion: 'Soporte técnico mensual',  cantidad: 3, precio:  45.00 },
    ]

    const subtotal = items.reduce((acc, i) => acc + i.cantidad * i.precio, 0)
    const iva      = parseFloat((subtotal * 0.15).toFixed(2))
    const total    = parseFloat((subtotal + iva).toFixed(2))

    return {
      numero:   `FAC-${String(id).padStart(6, '0')}`,
      fecha:    new Date().toLocaleDateString('es-EC'),
      cliente,
      items,
      subtotal,
      iva,
      total,
    }
  }
}
