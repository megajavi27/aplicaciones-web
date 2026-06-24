import { Routes } from '@angular/router';
import { Clientes } from './features/clientes/clientes';

export const routes: Routes = [
    {
        path:'',
        component:Clientes,
        pathMatch:'full'
        
    }
];
