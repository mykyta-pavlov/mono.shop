import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-number-increment',
  templateUrl: './number-increment.component.html',
  styleUrls: ['./number-increment.component.scss']
})
export class NumberIncrementComponent implements OnInit {
  @Input() quantity: number;
  @Input() minQuantity: number = 1;
  @Input() maxQuantity: number = 5;

  @Output() changeEvent = new EventEmitter();

  isIncrementButtonDisabled = false;
  isDecrementButtonDisabled = false;

  constructor() { }

  ngOnInit(): void {
    this.setDisabled();
  }

  increment(): void {
    if(this.isIncrementButtonDisabled) return;
    this.quantity++;
    this.changeEvent.emit(this.quantity);
    this.setDisabled();
  }

  decrement(): void {
    if(this.isDecrementButtonDisabled) return;
    this.quantity--;
    this.changeEvent.emit(this.quantity);
    this.setDisabled();
  }

  setDisabled(): void {
    this.isIncrementButtonDisabled = !(this.quantity < this.maxQuantity);
    this.isDecrementButtonDisabled = !(this.quantity > this.minQuantity); // TODO optimise it
  }
}
