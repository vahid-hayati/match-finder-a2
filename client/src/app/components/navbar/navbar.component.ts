import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { TestService } from '../../services/test.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    RouterModule, RouterLink,
    MatButtonModule, MatToolbarModule, MatIconModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  testServie = inject(TestService);

}
