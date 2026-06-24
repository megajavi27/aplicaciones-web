import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { ClientesModule } from './clientes/clientes.module';
import { TypeOrmModule } from '@nestjs/typeorm';
import {Cliente} from './clientes/entities/cliente.entity'

@Module({
  imports: [TypeOrmModule.forRoot({
    type:"mysql",
    host:"localhost",
    port:3306,
    username:"root",
    password:"root", //para mamp
    database:"SextoApiClientes",
    entities:[Cliente],
    synchronize:true,

  }),
    ClientesModule],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
