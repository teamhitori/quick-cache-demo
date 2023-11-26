import { NgModule, Injector } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { createCustomElement } from '@angular/elements';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { HttpClientModule } from '@angular/common/http';
import { NgxGaugeModule } from 'ngx-gauge';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    HttpClientModule,
    NgxGaugeModule
  ],
  exports: [
    AppComponent
  ],
  providers: []
})
export class AppModule { 
  constructor(private injector: Injector) {
  }

  ngDoBootstrap() {
    customElements.define('ng-app', createCustomElement(AppComponent,
      { injector: this.injector }));

    customElements.define('app-root', createCustomElement(AppComponent,
      { injector: this.injector }));
  }
}
