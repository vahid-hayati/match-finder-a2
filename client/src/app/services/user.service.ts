import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from '../models/member.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { UserUpdate } from '../models/user-update.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _http = inject(HttpClient);
  private readonly _baseApiUrl: string = environment.baseApiUrl + 'api/user/';

  updateUser(userInput: UserUpdate): Observable<ApiResponse> {
    return this._http.put<ApiResponse>(this._baseApiUrl + 'update-by-id', userInput);
  }

  setMainPhoto(url_165: string): Observable<ApiResponse> {
    let queryParams = new HttpParams().set('photoUrlIn', url_165);

    return this._http.put<ApiResponse>(this._baseApiUrl + 'set-main-photo', null, {
      params: queryParams
    });
  }

  deletePhoto(url_165: string): Observable<ApiResponse> {
    let queryParams = new HttpParams().set('photoUrlIn', url_165);

    return this._http.put<ApiResponse>(this._baseApiUrl + 'delete-photo', null, {
      params: queryParams
    })
  }
}