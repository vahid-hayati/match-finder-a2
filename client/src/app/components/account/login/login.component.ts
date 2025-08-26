import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AccountService } from '../../../services/account.service';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Login } from '../../../models/login.model';
import { Observable } from 'rxjs';
import { LoggedIn } from '../../../models/logged-in.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    RouterModule,
    FormsModule, ReactiveFormsModule,
    MatFormFieldModule, MatInputModule, MatButtonModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  accountService = inject(AccountService);
  fB = inject(FormBuilder);

  //#region loginFg
  loginFg = this.fB.group({
    userNameCtrl: ['', [Validators.required]],
    passwordCtrl: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]]
  });

  get UserNameCtrl(): FormControl {
    return this.loginFg.get('userNameCtrl') as FormControl;
  }

  get PasswordCtrl(): FormControl {
    return this.loginFg.get('passwordCtrl') as FormControl;
  }
  //#endregion

  login(): void {
    let userIn: Login = {
      userName: this.UserNameCtrl.value,
      password: this.PasswordCtrl.value
    }

    let loginRes$: Observable<LoggedIn | null> = this.accountService.login(userIn);

    loginRes$.subscribe({
      next: (res) => {
        console.log(res);
      }
    })
  }
}
