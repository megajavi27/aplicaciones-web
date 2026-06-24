import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { variables_ambiente} from '../../../environments/environment.developer'
import { Observable } from 'rxjs';
import { Icliente } from '../interfaces/icliente';
@Injectable({
  providedIn: 'root',
})
export class ClienteServices {

  
  apiurl=variables_ambiente.apiBaseUrl

  constructor(private readonly http:HttpClient) {
    
    
  }
  todos():Observable<Icliente[]>{
    return this.http.get<Icliente[]>(this.apiurl+'/clientes')
  }

  factura(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiurl}/clientes/${id}/factura`)
  }

}
