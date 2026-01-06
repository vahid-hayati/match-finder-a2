import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from '../models/member.model';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  http = inject(HttpClient);
  private readonly _baseApiUrl: string = environment.baseApiUrl + 'api/';

  getAll(): Observable<Member[]> {
    let response$: Observable<Member[]> =
      this.http.get<Member[]>(this._baseApiUrl + 'member/get-all');

    return response$;
  }

  getByUserName(userName: string): Observable<Member> {
    let response$: Observable<Member> =
      this.http.get<Member>(this._baseApiUrl + 'member/get-by-username/' + userName);

    return response$;
  }
}
