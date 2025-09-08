import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AppUser } from '../models/app-user.model';
import { map, Observable } from 'rxjs';
import { LoggedIn } from '../models/logged-in.model';
import { Login } from '../models/login.model';
import { Member } from '../models/member.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  http = inject(HttpClient);

  private readonly _baseApiUrl: string = 'http://localhost:5000/api/';

  // baseApiUrl: string = 'http://localhost:5000/api/';
  // private readonly _baseApiUrl: string = 'http://localhost:5000/api/';

  register(userInput: AppUser): Observable<LoggedIn | null> {
    let response$: Observable<LoggedIn | null> =
      this.http.post<LoggedIn>(this._baseApiUrl + 'account/register', userInput)
        .pipe(map(res => {
          if (res) {
            this.setCurrentUser(res);
          }

          return null;
        }))

    // let response$: Observable<LoggedIn> = this.http.post<LoggedIn>(this._baseApiUrl + 'account/register', userInput);
    return response$;
  }

  login(userInput: Login): Observable<LoggedIn | null> {
    let response$: Observable<LoggedIn | null> =
      this.http.post<LoggedIn>(this._baseApiUrl + 'account/login', userInput)
        .pipe(map(res => {
          if (res) {
            this.setCurrentUser(res);
          }

          return null
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


  setCurrentUser(userInput: LoggedIn): void {
    localStorage.setItem('loggedIn', JSON.stringify(userInput));
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
