
module.exports = {
    cache: {
        type: 'memory',
      },
    entry: {
                   app:'./app/app.js'
                   
            },
    output:
    {
        path: __dirname + '/dist',
        filename: '[name].js'
        
    },
        mode: 'development',
        watch: false,
        devtool: 'source-map',
        module: {
            rules: [
                {
                    test: /\.jsx?$/,
                    use: [
                        {
                            loader: 'babel-loader',
                            options: {
                                presets: ['@babel/preset-env', '@babel/preset-react']
                            },
                        }
                    ]
                 },
                 {
                    test: /\.css$/i,
                    use: ["style-loader", "css-loader"],
                  },
                ]
        },
        resolve: {
             modules: [__dirname, 'node_modules']
        }
};
