import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms'
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AppUser } from './models/app-user.model';
import { Login } from './models/login.model';
import { LoggedIn } from './models/logged-in.model';
import { Member } from './models/member.model';
import { AccountService } from './services/account.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    FormsModule, ReactiveFormsModule,
    MatFormField, MatInputModule, MatButtonModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  accountService = inject(AccountService);
  fB = inject(FormBuilder);
  userResponse: LoggedIn | undefined;
  error: string | undefined;
  users: Member[] | undefined;
  member: Member | undefined;
  private _userIn: string = '';
  arePasswordsMatch: boolean | undefined;

  //#region form group
  signUpFg = this.fB.group({
    emailCtrl: ['', [Validators.required, Validators.email]], // formControl
    userNameCtrl: ['', [Validators.required]],
    ageCtrl: [0, [Validators.required, Validators.min(18), Validators.max(70)]],
    passwordCtrl: ['', [Validators.minLength(4), Validators.maxLength(8)]],
    confirmPasswordCtrl: ['', [Validators.required]],
    genderCtrl: '',
    cityCtrl: '',
    countryCtrl: ''
  });

  get EmailCtrl(): FormControl {
    return this.signUpFg.get('emailCtrl') as FormControl;
  }

  get UserNameCtrl(): FormControl {
    return this.signUpFg.get('userNameCtrl') as FormControl;
  }

  get AgeCtrl(): FormControl {
    return this.signUpFg.get('ageCtrl') as FormControl;
  }

  get PasswordCtrl(): FormControl {
    return this.signUpFg.get('passwordCtrl') as FormControl;
  }

  get ConfirmPasswordCtrl(): FormControl {
    return this.signUpFg.get('confirmPasswordCtrl') as FormControl;
  }

  get GenderCtrl(): FormControl {
    return this.signUpFg.get('genderCtrl') as FormControl;
  }

  get CityCtrl(): FormControl {
    return this.signUpFg.get('cityCtrl') as FormControl;
  }

  get CountryCtrl(): FormControl {
    return this.signUpFg.get('countryCtrl') as FormControl;
  }
  //#endregion

  register(): void {
      let userInput: AppUser = {
        email: this.EmailCtrl.value,
        userName: this.UserNameCtrl.value,
        age: this.AgeCtrl.value,
        password: this.PasswordCtrl.value,
        confirmPassword: this.ConfirmPasswordCtrl.value,
        gender: this.GenderCtrl.value,
        city: this.CityCtrl.value,
        country: this.CountryCtrl.value
      }

      let response$: Observable<LoggedIn> =  this.accountService.register(userInput);

      response$.subscribe({
        next: (res) => {
          this.userResponse = res;
        },
        error: (err) => {
          this.error = err.error;
        }
      })
  }
}
