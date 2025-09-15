import { Component, Inject, inject, model } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink, RouterModule } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { Member } from '../../models/member.model';
import { Observable } from 'rxjs';
import { Car } from '../../models/car.model';
import { TestService } from '../../services/test.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    RouterModule,
    MatButtonModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  testService = inject(TestService);

  // name: string = '';

  // setLocalStorage(): void {
  //   let car: Car = {
  //     brand: 'Samand',
  //     model: 'CLS 500'
  //   }

  //   localStorage.setItem('car', JSON.stringify(car));

  //   // localStorage.setItem('name', 'Reyhane');
  // }

  // getValueLocalStorage(): void {
  //   let name: string | null = localStorage.getItem('name');

  //   console.log(name);
  // }

  // setLocalStorage2(): void {
  //   localStorage.setItem('age', '26');
  // }

  // getValueFromLocalStorage(): void {
  //   let ageValue: string | null = localStorage.getItem('age');

  //   console.log(ageValue);
  // }
}