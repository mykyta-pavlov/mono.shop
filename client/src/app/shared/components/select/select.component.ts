import { Component, Input, OnInit } from '@angular/core';
import { ISelectOption } from '../../models/selectOption';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss']
})
export class SelectComponent implements OnInit {
  @Input() options: ISelectOption[];

  constructor() { }

  ngOnInit(): void {
  }

}
