import {
  Component,
  OnInit,
  ViewEncapsulation,
  Input,
  Output,
  EventEmitter,
  Self,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { Observable } from 'rxjs';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-custom-select',
  templateUrl: './custom-select.component.html',
  styleUrls: ['./custom-select.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class CustomSelectComponent implements OnInit, ControlValueAccessor {
  @Input() label: string;
  @Input() items: Observable<any[]>;
  @Input() bindLabel: Observable<any[]>;
  @Input() bindValue: Observable<any[]>;

  @Output() keyupEvent = new EventEmitter();
  @Output() changeEvent = new EventEmitter();

  selectedValue: any;

  constructor(@Self() public controlDir: NgControl) {
    this.controlDir.valueAccessor = this;
  }

  // #region ControlValueAccessor
  @ViewChild('select', { static: true }) input: ElementRef;

  writeValue(obj: any): void {
    this.input.nativeElement.value = obj || '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  onTouched(): void {}

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  //#endregion

  onChange(target: EventTarget) {
    this.changeEvent.emit(target);
  }

  keyup(target: EventTarget) {
    this.keyupEvent.emit(target);
    console.log(this.items);
    
  }

  ngOnInit(): void {}
}
