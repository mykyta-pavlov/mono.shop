import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketItem } from '../../models/basket';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  basket$: Observable<IBasket>;
  @Output() changeEvent: EventEmitter<{item: IBasketItem, quantity: number}> 
    = new EventEmitter<{item: IBasketItem, quantity: number}>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Input() isBasket = true;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    console.log(this.basket$);
    this.basket$ = this.basketService.basket$;
  }

  changeItemQuantity(item: IBasketItem, quantity: number): void {
    this.changeEvent.emit({item, quantity});
  }

  removeBasketItem(item: IBasketItem): void {
    this.remove.emit(item);
  }

}
