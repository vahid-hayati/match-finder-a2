import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { Login } from '../../../models/login.model';
import { Observable } from 'rxjs';
import { LoggedIn } from '../../../models/logged-in.model';
import { RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterModule, 
    MatButtonModule,
    MatFormFieldModule, ReactiveFormsModule, MatInputModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  accountService = inject(AccountService);
  fB = inject(FormBuilder);
  loggedInRes: LoggedIn | undefined | null;

  //#region loginFg
  loginFg = this.fB.group({
    userNameCtrl: ['', [Validators.required]],
    passwordCtrl: ['', [Validators.minLength(4), Validators.maxLength(8)]]
  });

  get UserNameCtrl(): FormControl {
    return this.loginFg.get('userNameCtrl') as FormControl;
  }

  get PasswordCtrl(): FormControl {
    return this.loginFg.get('passwordCtrl') as FormControl;
  }
  //#endregion

  login(): void {
    let userInput: Login = {
      userName: this.UserNameCtrl.value,
      password: this.PasswordCtrl.value
    }

    let loginResponse$: Observable<LoggedIn | null> = this.accountService.login(userInput);

    loginResponse$.subscribe({
      next: (res => {
        console.log(res);
        this.loggedInRes = res;
      })
    });
  }
}
