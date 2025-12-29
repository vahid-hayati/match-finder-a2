import { Component, inject } from '@angular/core';
import { RouterLink, RouterModule } from '@angular/router';
import { TestService } from '../../services/test.service';
import { AccountService } from '../../services/account.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { CommonModule } from '@angular/common';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule, RouterLink,
    MatButtonModule, MatToolbarModule, MatIconModule,
    MatMenuModule, MatDividerModule, MatListModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  accountService = inject(AccountService);
  testService = inject(TestService);
  apiUrl = environment.baseApiUrl;

  logout(): void {
    this.accountService.logout();
  }
}
