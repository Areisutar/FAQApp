// vue/src/services/HttpClient.ts
import axios from "axios";
import type { AxiosInstance } from "axios";
import { type IHttpClient, type testModel } from "../interfaces/IHttpClient";

export class HttpClient implements IHttpClient {
    private axiosInstance: AxiosInstance;

    constructor() {
        this.axiosInstance = axios.create({
            baseURL: "/api",
            timeout: 5000,
        });
    }

    // インターフェースで定義したメソッドを実装
    public async testApi(testModel: testModel): Promise<void> {
        // C#側の [HttpPost] 属性が付いたアクションに飛ばすイメージ
        await this.axiosInstance.post("/Test", testModel);
        console.log("送信成功:", testModel);
    }
}