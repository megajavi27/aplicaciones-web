import { Column, CreateDateColumn, Entity, PrimaryGeneratedColumn, UpdateDateColumn } from "typeorm";

@Entity('clientes')
export class Cliente {
    @PrimaryGeneratedColumn()
    id!:number;
    @Column({length:10, unique:true})
    cedula!:string;
    @Column({length:150})
    nombres!:string
    @Column({length:150})
    direccion!:string
    @Column({length:150})
    telefono!:string
    @Column({length:150})
    correo!:string
    @CreateDateColumn()
    createAt!:Date
    @UpdateDateColumn()
    updateAt!:Date

}
