import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-short',
  templateUrl: './fetch-short.component.html'
})
export class FetchShortComponent {
  public shorturls: ShortUrl[] = [];
  private http: HttpClient;
  private baseUrl: string;
  public formData: any = {};

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.loadShortUrls();
  }

  loadShortUrls() {
    this.http.get<ShortUrl[]>(this.baseUrl + 'api/shorturl').subscribe(result => {
      this.shorturls = result;
    }, error => console.error(error));

  }

  postUrl() {
    const formData = new FormData();
    formData.append('originalUrl', this.formData.originalUrl);

    this.http.post(this.baseUrl + 'api/shorturl', formData)
      .subscribe((result) => {
        console.log('POST success:', result);
        this.shorturls.push(result as ShortUrl);
        this.formData.originalUrl = '';
      }, (error) => {
        console.error('POST error:', error);
      });
  }
}

interface ShortUrl {
  id: number,
  originalUrl: string,
  shortLink: string,
  createdDate: string,
  createdBy: ApplicationUser
}

interface ApplicationUser {
  id: string,
  userName: string
}
