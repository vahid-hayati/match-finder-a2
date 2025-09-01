import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AppUser } from '../models/app-user.model';
import { Observable } from 'rxjs';
import { LoggedIn } from '../models/logged-in.model';
import { Login } from '../models/login.model';
import { Member } from '../models/member.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  http = inject(HttpClient);

  register(userInput: AppUser): Observable<LoggedIn> {
    let response$: Observable<LoggedIn> = this.http.post<LoggedIn>('http://localhost:5000/api/account/register', userInput);

    return response$;
  }

  login(userInput: Login): Observable<LoggedIn> {
    console.log('ok', userInput);

    let response$: Observable<LoggedIn> = this.http.post<LoggedIn>('http://localhost:5000/api/account/login', userInput);

    return response$;
  }

  getAll(): Observable<Member[]> {
    let response$: Observable<Member[]> = this.http.get<Member[]>('http://localhost:5000/api/member/get-all');

    return response$;
  }

  getByUserName(userName: string): Observable<Member> {
    let response$: Observable<Member> = this.http.get<Member>('http://localhost:5000/api/account/get-by-username/' + userName);

    return response$;
  }

  updateById(userId: string, userInput: AppUser): Observable<Member> {
    let response$: Observable<Member> = this.http.put<Member>('http://localhost:5000/api/account/update/' + userId, userInput);

    return response$;
  }
}
