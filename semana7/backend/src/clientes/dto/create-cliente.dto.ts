import { IsString, Length, MaxLength } from "class-validator";

export class CreateClienteDto {
    @IsString()
    @Length(10,10)
    cedula!:string

    @IsString()
    @MaxLength(150)
    nombres!:string

    @IsString()
    @MaxLength(150)
    direccion!:string

    @IsString()
    @MaxLength(150)
    telefono!:string

    @IsString()
    @MaxLength(150)
    correo!:string

}
