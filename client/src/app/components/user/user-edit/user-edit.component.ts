import { Component, inject, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

import { LoggedInUser } from '../../../models/logged-in.model';
import { Member } from '../../../models/member.model';
import { UserUpdate } from '../../../models/user-update.model';
import { ApiResponse } from '../../../models/api-response.model';

import { UserService } from '../../../services/user.service';
import { MemberService } from '../../../services/member.service';
import { environment } from '../../../../environments/environment.development';

import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';

import { PhotoEditorComponent } from '../photo-editor/photo-editor.component';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule, ReactiveFormsModule,
    MatFormFieldModule, MatInputModule,
    MatTabsModule, MatCardModule, MatButtonModule,
    PhotoEditorComponent
  ],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.scss'
})
export class UserEditComponent implements OnInit {
  private _platformId = inject(PLATFORM_ID); // browser, server
  private _memberService = inject(MemberService);
  private _userService = inject(UserService);
  private _fB = inject(FormBuilder);
  private _snackbar = inject(MatSnackBar);

  apiUrl = environment.baseApiUrl;
  member: Member | undefined;

  readonly maxTextAreaChars: number = 1000;
  readonly minInputChars: number = 2;
  readonly maxInputChars: number = 30;

  ngOnInit(): void {
    this.getMember();

    console.log(this.member);

  }

  //#region userEditFg
  userEditFg = this._fB.group({
    introductionCtrl: ['', [Validators.maxLength(this.maxTextAreaChars)]],
    lookingForCtrl: ['', [Validators.maxLength(this.maxTextAreaChars)]],
    interestsCtrl: ['', [Validators.maxLength(this.maxTextAreaChars)]],
    cityCtrl: ['', [Validators.minLength(this.minInputChars), Validators.maxLength(this.maxInputChars)]],
    countryCtrl: ['', [Validators.minLength(this.minInputChars), Validators.maxLength(this.maxInputChars)]]
  });

  get IntroductionCtrl(): FormControl {
    return this.userEditFg.get('introductionCtrl') as FormControl;
  }
  get LookingForCtrl(): FormControl {
    return this.userEditFg.get('lookingForCtrl') as FormControl;
  }
  get InterestsCtrl(): FormControl {
    return this.userEditFg.get('interestsCtrl') as FormControl;
  }
  get CityCtrl(): FormControl {
    return this.userEditFg.get('cityCtrl') as FormControl;
  }
  get CountryCtrl(): FormControl {
    return this.userEditFg.get('countryCtrl') as FormControl;
  }
  //#endregion

  initialControllerValue(member: Member) {
    this.IntroductionCtrl.setValue(member.introduction);
    this.LookingForCtrl.setValue(member.lookingFor);
    this.InterestsCtrl.setValue(member.interests);
    this.CityCtrl.setValue(member.city);
    this.CountryCtrl.setValue(member.country);
  }

  getMember(): void {
    if (isPlatformBrowser(this._platformId)) {
      let loggedInUserStr: string | null = localStorage.getItem('loggedIn');

      if (loggedInUserStr) {
        let loggedInUserObj: LoggedInUser = JSON.parse(loggedInUserStr)

        this._memberService.getByUserName(loggedInUserObj.userName).subscribe({
          next: (res: Member) => {
            this.member = res;

            this.initialControllerValue(this.member);
            console.log(this.member);
          },
          error: (err) => {
            console.log(err);

          }
        })
      }
    }

  }

  updateUser(): void {
    let request: UserUpdate = {
      introduction: this.IntroductionCtrl.value,
      lookingFor: this.LookingForCtrl.value,
      interests: this.InterestsCtrl.value,
      city: this.CityCtrl.value,
      country: this.CountryCtrl.value
    }

    this._userService.updateUser(request).subscribe({
      next: (res: ApiResponse) => {
        this._snackbar.open(res.message, 'Close', {
          duration: 5000,
          verticalPosition: 'top',
          horizontalPosition: 'center'
        })

        this.userEditFg.markAsPristine();
      }
    })
  }
}