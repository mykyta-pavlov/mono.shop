import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { CheckoutService } from '../checkout.service';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { ISettlement } from '../../shared/models/settlements';
import { IWarehouse } from '../../shared/models/warehouse';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss'],
})
export class CheckoutAddressComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  deliveryMethods: IDeliveryMethod[];

  settlements$: Observable<ISettlement[]>;
  private searchSettlement$ = new Subject<string>();

  warehouses$: Observable<IWarehouse[]>;
  private searchWarehouse$ = new Subject<string>();

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private checkoutService: CheckoutService,
    private basketService: BasketService
  ) {}

  submitOrder(): void {
    // const basket = this.basketService.getCurrentBasketValue();
    // const orderToCreate = this.getOrderToCreate(basket);
    // this.checkoutService.createOrder(orderToCreate).subscribe((order: IOrder) => {
    //   this.toastr.success('Order created successfully');
    //   this.basketService.deleteLocalBasket(basket.id);
    //   const navigationExtras: NavigationExtras = {state: order};
    //   this.router.navigate(['checkout/success'], navigationExtras);
    // }, error => {
    //   this.toastr.error(error.message);
    //   console.log(error);
    // });
    console.log(this.checkoutForm.get('addressForm').get('deliveryCity').value);
  }

  ngOnInit(): void {
    this.settlements$ = this.searchSettlement$.pipe(
      debounceTime(1000),
      distinctUntilChanged(),
      switchMap((settlement) => {
        return this.checkoutService.getSettlements(settlement);
      })
    );

    this.warehouses$ = this.searchWarehouse$.pipe(
      debounceTime(1000),
      distinctUntilChanged(),
      switchMap((warehouse) => {
        return this.checkoutService.getWarehouses(warehouse);
      })
    );

    this.checkoutService.getDeliveryMethods().subscribe(
      (dm: IDeliveryMethod[]) => {
        this.deliveryMethods = dm;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  getSettlements(settlement: string): void {
    if (settlement) {
      console.log(settlement);

      this.searchSettlement$.next(settlement);
    }
  }

  getWarehouses(warehouse: string): void {
    if (warehouse) {
      this.searchWarehouse$.next(warehouse);
      console.log(this.warehouses$);
    }
  }

  saveUserAddress(): void {
    this.accountService
      .updateUserAddress(this.checkoutForm.get('addressForm').value)
      .subscribe(
        () => {
          this.toastr.success('Address saved');
        },
        (error) => {
          this.toastr.error(error.message);
          console.log(error);
        }
      );
  }

  setShippingPrice(deliveryMethod: IDeliveryMethod): void {
    this.basketService.setShippingPrice(deliveryMethod);
  }

  getValue(target: EventTarget): string {
    return (target as HTMLInputElement).value;
  }
}
