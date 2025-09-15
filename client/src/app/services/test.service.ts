import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TestService {
  nameSig = signal<string>('Hossein');

  changeSignalValue(): void {
    this.nameSig.set('Zeinab');
  }

  // nameSig = signal('Parsa');

  // changeName(): void {
  //   this.nameSig.set('Artemis');
  // }
}
