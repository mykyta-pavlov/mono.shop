import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IOrderToCreate } from '../shared/models/order';
import set = Reflect.set;
import { ISettlement } from '../shared/models/settlements';
import { IWarehouse } from '../shared/models/warehouse';

@Injectable({
  providedIn: 'root',
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  createOrder(order: IOrderToCreate): Observable<object> {
    return this.http.post(this.baseUrl + 'orders', order);
  }

  getSettlements(settlement: string): Observable<ISettlement[]> {
    return this.http.get<ISettlement[]>(
      this.baseUrl + 'novaposhta/searchsettlements/' + settlement
    );
  }

  getWarehouses(warehouse: string): Observable<IWarehouse[]> {
    return this.http.get<IWarehouse[]>(
      this.baseUrl + 'novaposhta/warehouses/' + warehouse
    );
  }

  getDeliveryMethods(): Observable<IDeliveryMethod[]> {
    return this.http.get(this.baseUrl + 'orders/deliveryMethods').pipe(
      map((dm: IDeliveryMethod[]) => {
        return dm.sort((a, b) => b.price - a.price);
      })
    );
  }
}
