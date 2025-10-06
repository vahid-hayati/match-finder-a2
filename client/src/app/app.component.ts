import { Component, inject, OnInit, PLATFORM_ID } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { AccountService } from './services/account.service';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet, RouterModule,
    NavbarComponent, FooterComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  accountService = inject(AccountService);
  platformId = inject(PLATFORM_ID);

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      let loggedInUserStr: string | null = localStorage.getItem('loggedIn');
      
      if (loggedInUserStr) {
        this.accountService.setCurrentUser(JSON.parse(loggedInUserStr));
      }
    }
  }
}