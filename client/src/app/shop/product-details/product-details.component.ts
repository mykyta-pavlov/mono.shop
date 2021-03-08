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

  constructor(private shopService: ShopService,
              private activatedRoute: ActivatedRoute,
              private bcService: BreadcrumbService,
              private basketService: BasketService) { }

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
    this.shopService.getProduct(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(product => {
      this.product = product;
      this.bcService.set('@productDetails', product.name);

      this.verticalSliderLength = product.imageUrls.length;
    }, error => {
      console.log(error);
    });
  }

  readonly marginTopIndent = 149;
  slideNumber = 0;
  verticalSliderLength;

  setSliderNumber(num: number): void {
    if(!((num < 0) || num >= this.verticalSliderLength)) {
      this.slideNumber = num;
    }
  }

}
