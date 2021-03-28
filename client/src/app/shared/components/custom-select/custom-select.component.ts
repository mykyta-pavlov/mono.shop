import {
  Component,
  OnInit,
  ViewEncapsulation,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-custom-select',
  templateUrl: './custom-select.component.html',
  styleUrls: ['./custom-select.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class CustomSelectComponent implements OnInit {
  @Input() label: string;
  @Input() items: Observable<any[]>;

  @Output() keyupEvent = new EventEmitter();

  keyup(target: EventTarget) {
    this.keyupEvent.emit(target);
  }

  constructor() {}

  ngOnInit(): void {}
}
