import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { error } from 'util';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancleRegistrationEvent = new EventEmitter();
  model: any = {};
  constructor(private authHttp: AuthService,private alertify: AlertifyService) { }

  ngOnInit() {
  }
  register() {
    console.log(this.model);
    this.authHttp.register(this.model).subscribe(next => {
      this.alertify.success('Registration Successfull');
    // tslint:disable-next-line: no-shadowed-variable
    }, error => {
      this.alertify.error(error);
    });
  }

  cencel() {
    this.cancleRegistrationEvent.emit(false);
  }
}
