import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { AppUser } from '../models/app-user.model';
import { map, Observable } from 'rxjs';
import { LoggedInUser } from '../models/logged-in.model';
import { Login } from '../models/login.model';
import { Member } from '../models/member.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  http = inject(HttpClient);
  router = inject(Router);
  loggedInUserSig = signal<LoggedInUser | null>(null);

  private readonly _baseApiUrl: string = 'http://localhost:5000/api/';

  // baseApiUrl: string = 'http://localhost:5000/api/';
  // private readonly _baseApiUrl: string = 'http://localhost:5000/api/';

  register(userInput: AppUser): Observable<LoggedInUser | null> {
    let response$: Observable<LoggedInUser | null> =
      this.http.post<LoggedInUser>(this._baseApiUrl + 'account/register', userInput)
        .pipe(map(response => {
          if (response) {
            this.setCurrentUser(response);
          }

          return null;
        }));

    // let response$: Observable<LoggedIn> = this.http.post<LoggedIn>(this._baseApiUrl + 'account/register', userInput);
    return response$;
  }

  login(userInput: Login): Observable<LoggedInUser | null> {
    let response$: Observable<LoggedInUser | null> =
      this.http.post<LoggedInUser>(this._baseApiUrl + 'account/login', userInput)
        .pipe(map(response => { // response: LoggedInUser
          if (response) {
            this.setCurrentUser(response);
          }

          return null;
        }));

    return response$;
  }

  getAll(): Observable<Member[]> {
    let response$: Observable<Member[]> =
      this.http.get<Member[]>(this._baseApiUrl + 'member/get-all');

    return response$;
  }

  getByUserName(userName: string): Observable<Member> {
    let response$: Observable<Member> =
      this.http.get<Member>(this._baseApiUrl + 'account/get-by-username/' + userName);

    return response$;
  }

  updateById(userId: string, userInput: AppUser): Observable<Member> {
    let response$: Observable<Member> =
      this.http.put<Member>(this._baseApiUrl + 'account/update/' + userId, userInput);

    return response$;
  }

  setCurrentUser(loggedInUser: LoggedInUser): void {
    this.loggedInUserSig.set(loggedInUser);

    localStorage.setItem('loggedIn', JSON.stringify(loggedInUser));
  }

  logout(): void {
    this.loggedInUserSig.set(null);

    localStorage.clear();

    this.router.navigateByUrl('account/login');
  }

  /*
    setCurrentUser(loggedIn: LoggedIn): void {
    this.loggedInUserSig.set(loggedIn);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('loggedInUser', JSON.stringify(loggedIn));
    }
  }
  */
}
