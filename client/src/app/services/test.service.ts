import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TestService {
  nameSig = signal("Parsa");

  changeName(): void {
    this.nameSig.set("Artemis");
  }
}
