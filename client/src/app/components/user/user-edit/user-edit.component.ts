import { Component, inject, OnInit, PLATFORM_ID } from '@angular/core';
import { MemberService } from '../../../services/member.service';
import { isPlatformBrowser } from '@angular/common';
import { LoggedInUser } from '../../../models/logged-in.model';
import { Member } from '../../../models/member.model';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.scss'
})
export class UserEditComponent implements OnInit {
  private _memberService = inject(MemberService);
  private _platformId = inject(PLATFORM_ID); // browser, server
  member: Member | undefined; 

  ngOnInit(): void {
      this.getMember();
  }

  getMember(): void {
    if (isPlatformBrowser(this._platformId)) {
      let loggedInUserStr: string | null = localStorage.getItem('loggedIn');

      if (loggedInUserStr) {
        let loggedInUserObj: LoggedInUser = JSON.parse(loggedInUserStr)

        this._memberService.getByUserName(loggedInUserObj.userName).subscribe({
          next: (res: Member) => {
            this.member = res;
            console.log(this.member);
          } 
        })
      }
    }

  }
}
