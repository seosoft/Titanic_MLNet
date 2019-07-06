# 予測サービスをクラウドに発行する

[**前のステップ**](./06_createfunctions.md) で、予測サービスを作成して、ローカルでデバッグ実行しました。

このステップではコンテンツの仕上げとして、

- 作成したサービスを Azure に発行
- クライアントから呼び出し

を行います。

> Azure サブスクリプションを持っていない場合は、[こちら](https://azure.microsoft.com/ja-jp/free/) で無償アカウントを作成してください。

---

## Azure にサインインする

1. アクティビティバーの [**Azure**] をクリックします。
2. （まだ Azure にサインインしていない場合のみ） [**Sign in to Azure**] でサインインします。  
   ![Sign in to Azure](./images/07/sign_in_to_azure.jpg)

3. サインインに成功した場合、または、すでにサインインしている場合には、アクセス可能なサブスクリプションが表示されます。  

   > Visual Studio Code に Functions 以外の Azure 関連の拡張機能をインストール済みの場合は、それらのセクションも表示されます。  
   > 以下の手順では [**AZURE: FUNCTIONS**] のセクションで操作します。

   ![Subscriptions in Functions Tab](./images/07/vscode_functions_subscriptions.jpg)  

> 複数のアカウントを持っていて、関数のデプロイ対象にしたいサブスクリプションが表示されていない場合は、いったんサインアウトします。
>
> 1. "**Ctrl+Shift+P**" で Visual Studio Code のコマンドパレットを開きます。
> 2. "**Azure: Sign Out**" を入力または選択して、サインアウトします。
>
> ![Signout](./images/07/signout_azure_vscode.jpg)

---

## Azure Functions に関数を作成する

[前のステップ](./06_createfunctions.md) でローカルでの実行に成功した予測サービスを Azure Functions にデプロイします。

1. [**Deploy to Function App**] をクリックします。  
   ![Deploy to Function App](./images/07/deploy_to_function_app.jpg)

2. [**Select a Subscription**] で、デプロイするサブスクリプションを選択します。  
   ![Select a Subscription](./images/07/select_subscription.jpg)

3. [**Create New Function App**] をクリックします。  
   ![Create New Function app](./images/07/create_new_function_app.jpg)

4. Function App の名前を入力します。  
   他のアプリケーションとは完全に異なる名前にする必要があるため、例えば "titanicfunc" などと、自分の名前、日付などを組み合わせて、名前を決めます。  
   ![Enter a unique name](./images/07/enter_unique_name.jpg)

5. これで Azure 上に Function App が作成されます。  
   少し待って、"Deployment to \<Function App 名\> competed" メッセージが表示されることを確認します。  
   ![Deploy to FuncApp Completed](./images/07/complete_deploy_function.jpg)

---

## Function App の設定変更

ML.NET のモデルは、64ビットのプラットフォームで実行する必要があります。

1. 作成した **関数で右クリック** して [**Open in Portal**] を選択します。  
   ![Open in Portal](./images/07/open_in_portal.jpg)

2. ブラウザーで Azure ポータルが開いたら、[**<作成した Function App>**]-[**プラットフォーム機能**] を選択します。  
   ![Select Platform](./images/07/select_platform_menu.jpg)]

3. [**全般設定**]-[**構成**] を選択します。  
   ![Select Configuration](./images/07/select_platform_config.jpg)

4. [**プラットフォームの設定**]-[**プラットフォーム**] を "**64 Bit**" に変更して、[**保存**] をクリックします。  
   ![Change the Platform to x64](./images/07/change_platform_to_x64.jpg)

---

## Blob に学習済みモデルファイルを配置する

学習済みモデルのファイルを **Blob** ストレージに配置します。  
再学習した際などにもモデルファイルを変更するだけでより精度の高い予測ができるようになるなどのメリットがあります。

1. Azure ポータルの検索窓で "titanic" を検索します。  
   "**ストレージアカウント**" が検索結果に表示されるので、これを選択します。

   > Function App 名として "titanic" 以外の文字列を使った場合は、それに応じた検索をします。

   ![Search Storage Account](./images/07/search_storageaccount.jpg)

2. [**BLOB**] を選択します。  
   ![Select Blob Storage](./images/07/select_blob_storage.jpg)

3. [**コンテナー**] を選択します。
4. [名前] に "**models**" を指定します。
5. [**パブリックアクセスレベル**] で "**BLOB (BLOB 専用の匿名読み取りアクセス)**"
6. [**OK**] をクリックして、コンテナーを作成します。  
   ![Create New Container](./images/07/create_container.jpg)

7. "**models**" ストレージアカウントを選択します。  
   ![Select models Blob Storage](./images/07/select_models_account.jpg)

8. [**アップロード**] を選択して、[モデルを作成する](./04_createmodel.md) ステップで保存した "TrainedModel.zip" ファイルをアップロードします。  
   ![Upload TrainedModel.zip File](./images/07/upload_trainedmodel_file.jpg)

9. アップロードに成功したら、"**TrainedModel.zip**" を選択します。
    ![Select TrainedModel.zip](./images/07/select_trainedmodel_zip.jpg)

10. "TrainedModel.zip" の [**URL**] をクリップボードにコピーします。  
    この値はあとで使います。

    ![Copy File URL to Clipboard](./images/07/copy_url_to_clopboard.jpg)

---

## ソースコードの一部変更

Blob にアップロードした学習済みモデルのファイル "TrainedModel.zip" を使用して予測するように、ソースコードを変更します。

1. Visual Studio Code に戻って、"**Startup.cs**" を開きます。
2. "**Configure**" メソッドを以下のように変更します。  

   ```csharp
   public void Configure(IWebJobsBuilder builder)
   {
       builder.Services.AddPredictionEnginePool<Passenger, PassengerPredict>()
           // .FromFile("Models/TrainedModel.zip");
           .FromUri("<Blob ストレージにアップロードした TrainedModel.zip の URL>");
   }
   ```

   ![Edit Startup.cs](./images/07/edit_configure_method.jpg)

---

## Function App にデプロイ

ソースコードを変更したので、Function App にデプロイします。

1. Azure に作成した Function App を右クリックして、[**Deploy to Function App**] を選択します。  
   ![Deploy to Function App](./images/07/deploy_to_function_app_again.jpg)

2. 確認ウィンドウが表示されたら [**Deploy**] を選択します。  
   ![Confirm to Deploy](./images/07/confirm_deploy.jpg)

---

## Azure ポータルで動作確認

予測サービスが Azure Functions で動作するようになりました。  
Azure ポータルで動作確認してみます。

1. Azure に作成した Function App を右クリックして、[**Open in Portal**] を選択します。  
   ![Open in Portal](./images/07/open_function_app_in_portal_again.jpg)

2. Azure ポータルで "**Function App ブレード**" が開いたら、[**関数 (読み取り専用)**] のドリルダウンを開いて、"**PredictSurvived**" を選択します。  
   続いて [**テスト**] タブをクリックして広げます。
   ![Select PredictSurvived Function](./images/07/select_predictsurvived_in_azure_portal.jpg)

3. [**要求本文**] に、例えば以下の JSON を入力します。  

   ```json
   {
       "Pclass": 1,
       "Sex": 0,
       "Age": 20,
       "SibSp": 1,
       "Parch": 0,
       "Fare": 30
   }
   ```

   ![Request Body on Portal](./images/07/input_request_body_on_portal.jpg)

4. [**実行**] をクリックします。  
   [**出力**] 領域に、予測した結果が表示されます。  
   ![Run Function](./images/07/run_function_on_portal.jpg)

Azure Functions 化に成功しました。

---

## 予測サービスを Postman から呼び出す

1. Azure ポータルの "**Function App ブレード**" で、[**関数 (読み取り専用)**] のドリルダウンを開いて、"**PredictSurvived**" を選択します。
2. [**関数の URL の取得**] をクリックします。  
   ![Get Function App URL](./images/07/get_function_url.jpg)

3. [コピー] をクリックして、URL をクリップボードにコピーします。  
   ![Copy Function App URL](./images/07/copy_function_url.jpg)

4. Postman を起動して、以下の必要な情報を埋めていきます。  
  
   |区分|項目|値|
   |---|---|---|
   |リクエスト|メソッド|POST|
   |リクエスト|URL|Function App の URL (Azure ポータルでコピーしたもの)|
   |Header|Content-Type|application/json|
   |Body|("Raw" に切り替えて)|以下のような JSON を入力（値は適当に他の値に変更して）|

   ```json
   {
       "Pclass": 1,
       "Sex": 0,
       "Age": 20,
       "SibSp": 1,
       "Parch": 0,
       "Fare": 30
   }
   ```

   > Postman で入力する情報は、[**予測をサービス化する**](./06_createfunctions.md) の "**予測サービスを Postman から呼び出す**" 手順を参考にしてください。  
   >今回違うのは **URL** のみです。

5. [**Send**] ボタンをクリックします。予測結果（"Survived" または "Not Survived"）が返ってきます。
   ![Send Request](./images/07/send_request.jpg)

---

以上で、このコンテンツはすべて終了です。

機械学習のデータの用意、学習、クラウドへの発行、クライアントからの利用について、実際に操作してみました。
[公式の Tutorial](https://dotnet.microsoft.com/learn/machinelearning-ai/ml-dotnet-get-started-tutorial/intro) など他の資料も参照して、機械学習の理解を深めてください。

このコンテンツへの意見、改善の提案などは、このリポジトリの Issue, Pull request でお知らせください。
