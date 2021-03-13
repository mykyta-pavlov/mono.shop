import { Component, HostBinding, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;
  quantity = 1;

  selectedValue: string;
  selectOptions = [
    // { id: 1, name: "S" },
    // { id: 2, name: "M" },
    // { id: 3, name: "L" },
    // { id: 4, name: "XL" },
    // { id: 5, name: "XXL" },
  ];

  constructor(
    private shopService: ShopService,
    private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private basketService: BasketService
  ) {}

  ngOnInit(): void {
    this.bcService.set('@productDetails', ' ');
    this.loadProduct();
  }

  getQuantity(quantity: number): void {
    this.quantity = quantity;
  }

  addItemToBasket(): void {
    this.basketService.addItemToBasket(this.product, this.quantity);
  }

  loadProduct(): void {
    this.shopService
      .getProduct(+this.activatedRoute.snapshot.paramMap.get('id'))
      .subscribe(
        (product) => {
          let keys = Object.keys(product.sizesAvailable);
          let crutchProduct = [];
          keys.forEach((key) => {
            crutchProduct.push({
              name: key,
              isAvailable: product.sizesAvailable[key],
            });
          });
          this.selectOptions = crutchProduct;
          this.selectedValue = crutchProduct[0].name; // TODO clear this pathetic crutch

          this.product = product;
          this.bcService.set('@productDetails', product.name);

          this.verticalSliderLength = product.imageUrls.length;
          console.log(this.product);
        },
        (error) => {
          console.log(error);
        }
      );
  }

  readonly marginTopIndent = 149;
  slideNumber = 0;
  verticalSliderLength;

  setSliderNumber(num: number): void {
    if (!(num < 0 || num >= this.verticalSliderLength)) {
      this.slideNumber = num;
    }
  }
}
