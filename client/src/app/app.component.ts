import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  users: any

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
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
