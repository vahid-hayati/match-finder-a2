import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
// npm install @ng2-file-upload --legacy-peer-deps

import { AccountService } from '../../../services/account.service';
import { environment } from '../../../../environments/environment.development';

import { Photo } from '../../../models/photo.model';
import { Member } from '../../../models/member.model';
import { LoggedInUser } from '../../../models/logged-in.model';

import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [
    CommonModule,
    FileUploadModule,
    MatCardModule, MatIconModule, MatButtonModule
  ],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.scss'
})
export class PhotoEditorComponent implements OnInit {
  @Input('memberIn') memberIn: Member | undefined;
  
  private _accountService = inject(AccountService);

  uploader: FileUploader | undefined;
  loggedInUser: LoggedInUser | null | undefined;
  apiUrl: string = environment.baseApiUrl;
  hasBaseDropZoneOver = false;

  ngOnInit(): void {
    this.loggedInUser = this._accountService?.loggedInUserSig();

    this.initializeUploader();
  }

  fileOverBase(event: boolean): void {
    this.hasBaseDropZoneOver = event;

    console.log(this.hasBaseDropZoneOver);
  }

  initializeUploader(): void {
    if (this.loggedInUser) {
      this.uploader = new FileUploader({
        url: this.apiUrl + 'api/user/add-photo',
        authToken: 'Bearer ' + this.loggedInUser.token,
        isHTML5: true,
        allowedFileType: ['image'], // webp, jpeg, jpg, png, img
        removeAfterUpload: true,
        autoUpload: false,
        maxFileSize: 4_000_000 //byte => 4mb
      });

      this.uploader.onAfterAddingFile = (file) =>
        file.withCredentials = false;

      this.uploader.onSuccessItem = (file, response) => {
        if (response) {
          const photo: Photo = JSON.parse(response);
          this.memberIn?.photos.push(photo);

          if (this.memberIn?.photos.length === 1) {
            this.setNavbarProfilePhoto(photo.url_165);
          }
        }
      }
    }
  }

  setNavbarProfilePhoto(url_165: string): void {
    if (this.loggedInUser) {
      this.loggedInUser.profilePhotoUrl = url_165;

      this._accountService.setCurrentUser(this.loggedInUser);
    }
  }
}