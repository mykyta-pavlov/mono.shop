import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss'],
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddressFormValues();
  }

  createCheckoutForm(): void {
    this.checkoutForm = this.fb.group({
      addressForm: this.fb.group({
        // firstName: [null, Validators.required],
        // lastName: [null, Validators.required],
        // street: [null, Validators.required],
        deliveryCity: [null, Validators.required],
        warehouse: [null, Validators.required],
        phone: [null, Validators.required],
        // deliveryMethod: [null, Validators.required],
      }),
      // deliveryForm: this.fb.group({
      //   deliveryMethod: [null, Validators.required],
      // }),
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required],
      }),
    });
  }

  getAddressFormValues(): void {
    this.accountService.getUserAddress().subscribe(
      (address) => {
        if (address) {
          this.checkoutForm.get('addressForm').patchValue(address);
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
