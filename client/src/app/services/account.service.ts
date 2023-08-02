import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs'
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = "http://localhost:5172/api/v1/accounts"
  private currentUserSource = new BehaviorSubject<User | null>(null)
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + "/login", model).pipe(
      map((response: User) => {
        const user = response
        if (user) {
          localStorage.setItem("user", JSON.stringify(user))
          this.currentUserSource.next(user)
        }
      })
    )
  }

  logout() {
    localStorage.removeItem("user")
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user)
  }
}
