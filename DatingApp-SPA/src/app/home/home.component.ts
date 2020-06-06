import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registermode = false;
  constructor() { }

  ngOnInit() {
  }

  registerToggle() {
    this.registermode = !this.registermode;
  }

  registrationCancle(cancleMode: boolean) {
    this.registermode = cancleMode;
  }
}
