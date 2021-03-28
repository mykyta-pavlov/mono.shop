import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';

@Component({
  selector: 'app-custom-select',
  templateUrl: './custom-select.component.html',
  styleUrls: ['./custom-select.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class CustomSelectComponent implements OnInit {
  @Input() label;
  @Input() items;

  constructor() {}

  ngOnInit(): void {}
}
