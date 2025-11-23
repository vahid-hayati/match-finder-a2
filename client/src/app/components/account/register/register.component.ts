import { Component, inject } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoggedInUser } from '../../../models/logged-in.model';
import { Observable, Subscription } from 'rxjs';
import { AppUser } from '../../../models/app-user.model';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterLink, RouterModule } from '@angular/router';
import { MatDatepickerModule } from '@angular/material/datepicker';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    RouterModule, RouterLink,
    FormsModule, ReactiveFormsModule, MatFormFieldModule,
    MatButtonModule, MatInputModule, MatDatepickerModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  accountService = inject(AccountService);
  fB = inject(FormBuilder);

  userResponse: LoggedInUser | undefined | null;
  error: string | undefined;
  subscribedRegisterUser: Subscription | undefined;

  minDate = new Date();
  maxDate = new Date();

  ngOnInit(): void {
    const currentYear = new Date().getFullYear();
    this.minDate = new Date(currentYear - 99, 0, 1);
    this.maxDate = new Date(currentYear - 18, 0, 1);
  }

  ngOnDestroy(): void {
    this.subscribedRegisterUser?.unsubscribe();
  }

  //#region 
  registerFg = this.fB.group({
    emailCtrl: ['', [Validators.required, Validators.email]], // formControl
    userNameCtrl: ['', [Validators.required]],
    dateOfBirthCtrl: ['', [Validators.required]],
    passwordCtrl: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
    confirmPasswordCtrl: ['', [Validators.required]],
    genderCtrl: '',
    cityCtrl: '',
    countryCtrl: ''
  });

  get EmailCtrl(): FormControl {
    return this.registerFg.get('emailCtrl') as FormControl;
  }

  get UserNameCtrl(): FormControl {
    return this.registerFg.get('userNameCtrl') as FormControl;
  }

  get DateOfBirthCtrl(): FormControl {
    return this.registerFg.get('dateOfBirthCtrl') as FormControl;
  }

  get PasswordCtrl(): FormControl {
    return this.registerFg.get('passwordCtrl') as FormControl;
  }

  get ConfirmPasswordCtrl(): FormControl {
    return this.registerFg.get('confirmPasswordCtrl') as FormControl;
  }

  get GenderCtrl(): FormControl {
    return this.registerFg.get('genderCtrl') as FormControl;
  }

  get CityCtrl(): FormControl {
    return this.registerFg.get('cityCtrl') as FormControl;
  }

  get CountryCtrl(): FormControl {
    return this.registerFg.get('countryCtrl') as FormControl;
  }
  //#endregion

  getDateOnly(dob: string | null): string | undefined {
    if (!dob) return undefined;

    let theDob: Date = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())).toISOString().slice(0, 10);
    // gets the first 10 chars from this date YYYY-MM-DDTHH:mm:ss.sssZ the output is YYYY-MM-DD
  }

  sliceString(): void {
    let fullName: string = "Parsa Jafary"

    let name: string = fullName.slice(0, 5);

    console.log(name);
  }

  // getDateOfBirth(): void {
  //   let dobString = this.DateOfBirthCtrl.value;

  //   let dateObj = new Date(dobString);

  //   let dateObjUtc = dateObj.setMinutes(dateObj.getMinutes() - dateObj.getTimezoneOffset());

  //   let isoString = dateObj.toISOString();

  //   let dateOnly = isoString.slice(0, 10);

  //   console.log(dobString);
  //   console.log(dateObjUtc);
  //   console.log(isoString);
  //   console.log(dateOnly);
  // }

  register(): void {
    let userInput: AppUser = {
      email: this.EmailCtrl.value,
      userName: this.UserNameCtrl.value,
      dateOfBirth: this.getDateOnly(this.DateOfBirthCtrl.value),
      password: this.PasswordCtrl.value,
      confirmPassword: this.ConfirmPasswordCtrl.value,
      gender: this.GenderCtrl.value,
      city: this.CityCtrl.value,
      country: this.CountryCtrl.value
    }

    let response$: Observable<LoggedInUser | null> = this.accountService.register(userInput);

    this.subscribedRegisterUser = response$.subscribe({
      next: (res) => {
        console.log(res);
        this.userResponse = res;
      },
      error: (err) => {
        console.log(err.error);
        this.error = err.error;
      }
    });
  }
}
