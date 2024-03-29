import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/public/login/login.component';
import { HomeComponent } from './components/secure/home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BasicAuthInterceptor } from '../app/shared/common/basic-auth.interceptor';
import { ErrorInterceptor } from '../app/shared/common/error.interceptor';
import { AgGridModule } from 'ag-grid-angular';
import { UserComponent } from './components/secure/user/user.component';
import { ClientDealerMappingComponent } from './components/secure/client-dealer-mapping/client-dealer-mapping.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ManageUserComponent } from './components/secure/manage-user/manage-user.component';
import { NetPositionViewComponent } from './components/secure/net-position-view/net-position-view.component';
import { DashboardComponent } from './components/secure/dashboard/dashboard.component';
import { ChartsModule } from 'ng2-charts';

import { ArbitragePositionComponent } from './components/secure/arbitrage-position/arbitrage-position.component';
import { GroupDealerMappingComponent } from './components/secure/group-dealer-mapping/group-dealer-mapping-component';
import { SupServicesComponent } from './components/secure/sup-services/sup-services.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    UserComponent,
    ClientDealerMappingComponent,
    ManageUserComponent,
    NetPositionViewComponent,
    DashboardComponent,
    ArbitragePositionComponent,
    GroupDealerMappingComponent,
    SupServicesComponent,
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AgGridModule.withComponents([]),

    DragDropModule,
    ChartsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
