export interface IHttpClient {
    testApi: (testModel:testModel) => Promise<void>;
}

export interface testModel{
    id?: number;
    text: string;
}
