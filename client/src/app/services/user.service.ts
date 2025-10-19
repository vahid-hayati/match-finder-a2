import { inject, Injectable } from '@angular/core';
import { AppUser } from '../models/app-user.model';
import { Observable } from 'rxjs';
import { Member } from '../models/member.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  http = inject(HttpClient);
  private readonly _baseApiUrl: string = environment.baseApiUrl + 'api/';

  updateById(userId: string, userInput: AppUser): Observable<Member> {
    let response$: Observable<Member> =
      this.http.put<Member>(this._baseApiUrl + 'account/update/' + userId, userInput);

    return response$;
  }
}
