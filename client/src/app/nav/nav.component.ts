import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { User } from '../models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}

  constructor(public accountService: AccountService) {}

  ngOnInit(): void {}

  login() {
    console.log(this.model)
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response)
      },
      error: error => console.log(error)
    })
  }

  logout() {
    this.accountService.logout()
  }
}
