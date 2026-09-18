export interface IHttpClient {
    testApi: (testModel:testModel) => Promise<void>;
    formApi: (formModel:formModel) => Promise<void>;
}

export interface testModel{
    id?: number;
    text: string;
}

export interface formModel{
    id?: number;
    gender: string;
    favoriteSns: string;
    description: string;
}
