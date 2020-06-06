import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { error } from 'util';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancleRegistrationEvent = new EventEmitter();
  model: any = {};
  constructor(private authHttp: AuthService) { }

  ngOnInit() {
  }
  register() {
    console.log(this.model);
    this.authHttp.register(this.model).subscribe(next => {
      console.log('Registration Successfull');
    // tslint:disable-next-line: no-shadowed-variable
    }, error => {
      console.log(error);
    });
  }

  cencel() {
    this.cancleRegistrationEvent.emit(false);
  }
}
