import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { CheckoutService } from '../checkout.service';
import {Observable, Subject} from 'rxjs';
import {debounceTime, distinctUntilChanged, switchMap} from 'rxjs/operators';
import {ISettlement} from '../../shared/models/settlements';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  deliveryMethods: IDeliveryMethod[];
  selectedValue = 1;
  settlements$: Observable<ISettlement[]>;
  private searchSettlement$ = new Subject<string>();

  constructor(private accountService: AccountService,
              private toastr: ToastrService,
              private checkoutService: CheckoutService,
              private basketService: BasketService) { }

  ngOnInit(): void {
    this.settlements$ = this.searchSettlement$.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      switchMap(settlement =>
        this.checkoutService.getSettlements(settlement))
    );

    this.checkoutService.getDeliveryMethods().subscribe((dm: IDeliveryMethod[]) => {
      this.deliveryMethods = dm;
    }, error => {
      console.log(error);
    });
  }

  getSettlements(settlement: string): void {
    if (settlement) {
      this.searchSettlement$.next(settlement);
    }
  }

  saveUserAddress(): void {
    this.accountService.updateUserAddress(this.checkoutForm.get('addressForm').value).subscribe(() => {
      this.toastr.success('Address saved');
    }, error => {
      this.toastr.error(error.message);
      console.log(error);
    });
  }

  setShippingPrice(deliveryMethod: IDeliveryMethod): void {
    this.basketService.setShippingPrice(deliveryMethod);
  }

  getValue(target: EventTarget): string {
    return (target as HTMLInputElement).value;
  }
}
