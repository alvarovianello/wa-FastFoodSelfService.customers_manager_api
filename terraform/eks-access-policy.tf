resource "aws_eks_access_policy" "eks-access-policy-customer"{
    cluster_name = aws_eks_cluster.eks-cluster-customer.name
    policy_arn = var.policyArn
    principal_arn = var.principalArn

    access_scope {
        type = "cluster"
    }
}