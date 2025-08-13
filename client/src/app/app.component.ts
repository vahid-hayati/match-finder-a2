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

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    FormsModule, ReactiveFormsModule,
    MatFormField, MatInputModule, MatButtonModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  http = inject(HttpClient); // post, get, put, delete
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
    if (this.PasswordCtrl.value === this.ConfirmPasswordCtrl.value) {
      this.arePasswordsMatch = true;

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

      this.http.post<LoggedIn>
        ('http://localhost:5000/api/account/register', userInput).subscribe({
          next: (response) => {
            console.log(response);
            this.userResponse = response
          },
          error: (apiError) => {
            this.error = apiError.error
            console.log(apiError)
          }
        });
    }
    else {
      this.arePasswordsMatch = false;
    }

  }

  login(): void {
    let userIn: Login = {
      userName: this.UserNameCtrl.value,
      password: this.PasswordCtrl.value
    }

    this.http.post('http://localhost:5000/api/account/login', userIn).subscribe({
      next: (res) => console.log(res)
    });
  }

  getAll(): void {
    this.http.get<Member[]>
      ('http://localhost:5000/api/account/get-all').subscribe({
        next: (response) => {
          this.users = response
          console.log(response)
        }
      });
  }

  getByUserName(): void {
    this._userIn = this.UserNameCtrl.value;

    this.http.get<Member>
      ('http://localhost:5000/api/account/get-by-username/' + this._userIn).subscribe({
        next: (res) => {
          console.log(res);
          this.member = res
        },
        error: (apiError) => {
          console.log(apiError.error)
          this.error = apiError.error
        }
      });
  }

  updateUserById(): void {
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

    this.http.put<Member>
      ('http://localhost:5000/api/account/update-by-id/6862bc3f1245f9f24d26beb7', userInput).subscribe({
        next: (res) => {
          console.log(res)
          this.member = res
        }
      });

    // this.http.put<Member>
    //   ('http://localhost:5000/api/account/update-by-id/6862bcba1245f9f24d26beb9', userInput).subscribe({
    //     next: (res) => {
    //       console.log(res);
    //       this.member = res;
    //     },
    //     error: (apiError) => {
    //       console.log(apiError.error);
    //       this.error = apiError.error;
    //     }
    //   });
  }

  deleteUserById(): void {
    this.http.delete
      ('http://localhost:5000/api/account/delete/689ca1237c19f708b3960cd1 ').subscribe();
  }
}
