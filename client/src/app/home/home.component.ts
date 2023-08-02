import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false
  users: any

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getUsers()
  }

  registerToggle() {
    this.registerMode = !this.registerMode
  }

  getUsers() {
    this.http.get("http://localhost:5172/api/v1/users").subscribe({
      next: response => {
        this.users = response;
        console.log(response)
      },
      error: err => console.log(err),
      complete: () => console.log("done!")
    })
  }
}
