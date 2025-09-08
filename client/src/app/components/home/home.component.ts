import { Component, Inject, inject, model } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink, RouterModule } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { Member } from '../../models/member.model';
import { Observable } from 'rxjs';
import { Car } from '../../models/car.model';

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
  name: string = '';

  setLocalStorage(): void {
    let car: Car = {
      brand: 'Samand',
      model: 'CLS 500'
    }

    localStorage.setItem('car', JSON.stringify(car));

    // localStorage.setItem('name', 'Reyhane');
  }

  getValueLocalStorage(): void {
    let name: string | null = localStorage.getItem('name');

    console.log(name);
  }

  setLocalStorage2(): void {
    localStorage.setItem('age', '26');
  }

  getNationalCode(): void {
    let nationalCode: string | null = localStorage.getItem('nationalCode');

    console.log('in', nationalCode, 'code meli shomast.');
  }

  setLocalSCopy(): void {
    localStorage.setItem('nationalCode', '102334466775588009933');
  }

  removeOneItemOfLocalStorage(): void {
    localStorage.removeItem('nationalCode');
  }

  clearLocalStorage(): void {
    localStorage.clear();
  }

  getLocalStorageLength(): void {
    let length: number = localStorage.length

    console.log(length);
  }

  getLocalStorageKey(): void {
    let key: string | null = localStorage.key(0);

    console.log(key);
  }

  // name: string | null | undefined;

  // setLocalStorage(): void {
  //   localStorage.setItem('name', 'parsa');
  // }

  // getLocalStorage(): void {
  //   // this.name = localStorage.getItem('name');
  //   let name: string | null = localStorage.getItem('name');

  //   console.log(name);
  // }
}