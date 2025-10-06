import { isPlatformBrowser } from '@angular/common';
import { HttpInterceptorFn } from '@angular/common/http';
import { inject, PLATFORM_ID } from '@angular/core';
import { LoggedInUser } from '../models/logged-in.model';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const platformId = inject(PLATFORM_ID); //server, browser

  if(isPlatformBrowser(platformId)) {
    const loggedInUserStr: string | null = localStorage.getItem('loggedIn');

    if (loggedInUserStr) {
      const loggedInUser: LoggedInUser = JSON.parse(loggedInUserStr);

      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${loggedInUser.token}`
        }
      })
    }
  }
  
  return next(req);
};
